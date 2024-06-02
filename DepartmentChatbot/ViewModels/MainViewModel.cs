using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DepartmentChatbot.Models;
using DepartmentChatbot.Services;
using System.Collections.ObjectModel;

namespace DepartmentChatbot.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        CancellationTokenSource? textToSpeechCancellationTokenSource;
        readonly MessageService messageService;

        [ObservableProperty]
        ObservableCollection<Message>? items;

        [ObservableProperty]
        ObservableCollection<Message>? itemsEN;

        [ObservableProperty]
        public string? text;

        [ObservableProperty]
        public bool isEnabled = false;

        [ObservableProperty]
        public bool canBeSent = true;

        [ObservableProperty]
        public bool isAnimated = false;

        public MainViewModel(MessageService messageService)
        {
            Items = [];
            ItemsEN = [];
            this.messageService = messageService;

            Message message = new()
            {
                Content = "Cześć! Jestem MikoAI i chętnie odpowiem na wszystkie twoje pytania!",
                Role = "assistant",
                IsSent = false
            };
            Items.Add(message);
            Message message2 = new()
            {
                Content = "Hi, I'm MikoAI and I'm happy to answer all your questions!",
                Role = "assistant",
                IsSent = false
            };
            ItemsEN.Add(message2);
        }

        static async Task PutTaskDelay()
        {
            await Task.Delay(3000);
        }


        [RelayCommand(CanExecute = nameof(CanManageFAQ))]
        void AcceptFAQ(Message msg)
        {
            msg.IsFAQ = false;
            IsEnabled = false;

            //just answer yes
            try
            {
                for (int i = Items.Count - 1; i >= 0; i--)
                {
                    if (Items[i] == msg)
                    {
                        Message yesAnswer = new()
                        {
                            Content = "Tak!",
                            IsSent = true,
                            Role = "user"
                        };
                        Items.Insert(i, yesAnswer);
                        Message yesAnswerEN = new()
                        {
                            Content = "Yes!",
                            IsSent = true,
                            Role = "user"
                        };
                        ItemsEN.Insert(i, yesAnswerEN);
                        break;
                    }
                }
            }
            catch
            {
            }

            CanBeSent = true;
            AcceptFAQCommand.NotifyCanExecuteChanged();
            RejectFAQCommand.NotifyCanExecuteChanged();
            SendCommand.NotifyCanExecuteChanged();
        }
        [RelayCommand(CanExecute = nameof(CanManageFAQ))]
        async Task RejectFAQ(Message msg)
        {
            try
            {
                for (int i = Items.Count - 1; i >= 0; i--)
                {
                    if (Items[i] == msg)
                    {
                        Items.RemoveAt(i);
                        Items.RemoveAt(i - 1);
                        ItemsEN.RemoveAt(i);
                        ItemsEN.RemoveAt(i - 1);
                        break;
                    }
                }
                IsEnabled = false;
                CanBeSent = true;
                AcceptFAQCommand.NotifyCanExecuteChanged();
                RejectFAQCommand.NotifyCanExecuteChanged();
                SendCommand.NotifyCanExecuteChanged();

                IsAnimated = true;
                //LLM
                if (await GetLLM() == false)
                {
                    IsAnimated = false;
                    return;
                }

                //translation
                if (await TranslateFromENToPL() == false)
                {
                    IsAnimated = false;
                    return;
                }
            }
            catch
            {
                await Shell.Current.DisplayAlert("Błąd!", "Coś poszło nie tak...", "OK");
            }

        }
        private bool CanManageFAQ()
        {
            return IsEnabled;
        }
        private bool CanSend()
        {
            return CanBeSent;
        }

        async Task<bool> TranslateFromPLToEN()
        {
            try
            {
                Message messageTranslated = new()
                {
                    Role = "user",
                    IsSent = true,
                    Content = await messageService.TranslateMessage(Items, "pl", "en-US")
                };
                ItemsEN.Add(messageTranslated);
                return true;
            }
            catch (HttpRequestException)
            {
                await Shell.Current.DisplayAlert("Błąd!", "Coś poszło nie tak...Sprawdź połączenie z internetem.", "OK");
                return false;
            }
            catch (Exception)
            {
                await Shell.Current.DisplayAlert("Błąd!", "Coś poszło nie tak...", "OK");
                return false;
            }
            finally
            {
                Text = string.Empty;
            }
        }

        async Task<bool> Categorize()
        {
            try
            {
                ClassifyResponse classifyResponse = new();
                classifyResponse = await messageService.GetCategory(ItemsEN);
                switch (classifyResponse.label)
                {
                    case "-1":
                        return true;
                    case "navigation":
                        //implementation deleted
                        return false;
                    case "chat":
                        //default
                        return true;
                    default:
                        await Shell.Current.DisplayAlert("Błąd!", "Coś poszło nie tak...", "OK");
                        return false;
                }
            }
            catch (HttpRequestException)
            {
                await Shell.Current.DisplayAlert("Błąd!", "Coś poszło nie tak...Sprawdź połączenie z internetem.", "OK");
                return false;
            }
            catch (Exception)
            {
                await Shell.Current.DisplayAlert("Błąd!", "Coś poszło nie tak...", "OK");
                return false;
            }
            finally
            {
                Text = string.Empty;
            }
        }

        async Task<bool> GetFaq()
        {
            try
            {
                List<TableContext> tableContexts = new List<TableContext>();
                tableContexts = await messageService.GetMessageFromFAQ(ItemsEN);
                if (tableContexts[0].AnswerPL != "-1")
                {
                    Message faqQuestionEN = new()
                    {
                        Content = "Did you mean...\n" + tableContexts[0].QuestionEN,
                        IsSent = false,
                        Role = "assistant"
                    };
                    ItemsEN.Add(faqQuestionEN);

                    Message faqAnswerEN = new()
                    {
                        Content = "Answer: \n" + tableContexts[0].AnswerEN,
                        IsSent = false,
                        Role = "assistant"
                    };
                    ItemsEN.Add(faqAnswerEN);

                    Message faqQuestionPL = new()
                    {
                        Content = "Czy chodziło ci o...\n" + tableContexts[0].QuestionPL,
                        IsSent = false,
                        Role = "assistant"
                    };
                    IsAnimated = false;
                    Items.Add(faqQuestionPL);

                    Message faqAnswerPL = new()
                    {
                        Content = "Odpowiedź: \n" + tableContexts[0].AnswerPL,
                        IsSent = false,
                        Role = "assistant",
                        IsFAQ = true
                    };
                    Items.Add(faqAnswerPL);

                    IsEnabled = true;
                    AcceptFAQCommand.NotifyCanExecuteChanged();
                    RejectFAQCommand.NotifyCanExecuteChanged();
                    CanBeSent = false;
                    SendCommand.NotifyCanExecuteChanged();
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (HttpRequestException)
            {
                await Shell.Current.DisplayAlert("Błąd!", "Coś poszło nie tak...Sprawdź połączenie z internetem.", "OK");
                return false;
            }
            catch (Exception)
            {
                await Shell.Current.DisplayAlert("Błąd!", "Coś poszło nie tak...", "OK");
                return false;
            }
            finally
            {
                Text = string.Empty;
            }
        }

        async Task<bool> GetLLM()
        {
            try
            {
                Message messageReceived = new()
                {
                    Role = "assistant",
                    IsSent = false,
                    Content = await messageService.GetMessageFromMain(ItemsEN)
                };
                ItemsEN.Add(messageReceived);
                return true;
            }
            catch (HttpRequestException)
            {
                await Shell.Current.DisplayAlert("Błąd!", "Coś poszło nie tak...Sprawdź połączenie z internetem.", "OK");
                return false;
            }
            catch (Exception)
            {
                await Shell.Current.DisplayAlert("Błąd!", "Coś poszło nie tak...", "OK");
                return false;
            }
            finally
            {
                Text = string.Empty;
            }
        }

        async Task<bool> TranslateFromENToPL()
        {
            try
            {
                Message messageTranslatedFromEN = new()
                {
                    Role = "user",
                    IsSent = false,
                    Content = await messageService.TranslateMessage(ItemsEN, "en", "pl")
                };
                IsAnimated = false;
                Items.Add(messageTranslatedFromEN);
                return true;
            }
            catch (HttpRequestException)
            {
                await Shell.Current.DisplayAlert("Błąd!", "Coś poszło nie tak...Sprawdź połączenie z internetem.", "OK");
                return false;
            }
            catch (Exception)
            {
                await Shell.Current.DisplayAlert("Błąd!", "Coś poszło nie tak...", "OK");
                return false;
            }
            finally
            {
                Text = string.Empty;
            }
        }

        [RelayCommand(CanExecute = nameof(CanSend))]
        async Task Send()
        {
            if (string.IsNullOrEmpty(Text))
                return;
            Message message = new()
            {
                Content = Text,
                IsSent = true,
                Role = "user"
            };
            Items.Add(message);
            IsAnimated = true;

            //translation
            if (await TranslateFromPLToEN() == false)
            {
                IsAnimated = false;
                return;
            }

            //categorization
            if (await Categorize() == false)
            {
                IsAnimated = false;
                return;
            }

            //FAQ
            if (await GetFaq() == false)
            {
                IsAnimated = false;
                return;
            }

            //LLM
            if (await GetLLM() == false)
            {
                IsAnimated = false;
                return;
            }

            //translation
            if (await TranslateFromENToPL() == false)
            {
                IsAnimated = false;
                return;
            }
        }

        [RelayCommand]
        async Task Tapped(string content)
        {
            if (textToSpeechCancellationTokenSource != null)
            {
                textToSpeechCancellationTokenSource.Cancel();
                textToSpeechCancellationTokenSource.Dispose();
                textToSpeechCancellationTokenSource = null;
            }
            else
            {
                textToSpeechCancellationTokenSource = new CancellationTokenSource();
                Message message = new()
                {
                    Content = content,
                    IsSent = true,
                    Role = "user"
                };
                await TextToSpeechService.ReadTheMessageAloud(message, textToSpeechCancellationTokenSource.Token);
            }
        }

        [RelayCommand]
        async Task Speak()
        {
            CancellationTokenSource source = new();
            CancellationToken token = source.Token;
            try
            {
                await SpeechToTextService.StartListening(token, this);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task DeleteHistory()
        {
            bool isYesChosen = await Shell.Current.DisplayAlert("Uwaga!", "Czy na pewno chcesz usunąć historię?", "Tak", "Nie");
            if (isYesChosen)
            {
                Items.Clear();
                ItemsEN.Clear();

                IsAnimated = true;
                await Task.Delay(1000);
                IsAnimated = false;

                Message message = new()
                {
                    Content = "Cześć! Jestem MikoAI i chętnie odpowiem na wszystkie twoje pytania!",
                    Role = "assistant",
                    IsSent = false
                };
                Items.Add(message);
                Message message2 = new()
                {
                    Content = "Hi, I'm MikoAI and I'm happy to answer all your questions!",
                    Role = "assistant",
                    IsSent = false
                };
                ItemsEN.Add(message2);
            }
        }
    }
}
