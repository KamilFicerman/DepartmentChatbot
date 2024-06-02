# MikoAI Frontend Project

**Note:** This repository contains only a part of the MikoAI project, which I developed individually. The entire project was a group effort for the course "Team Programming." This repository includes only the elements I personally worked on and does not encompass the full application. Below, you will find videos showcasing the final product.

## Overview

MikoAI is an innovative mobile application designed to simplify daily life at our faculty. The main features of the application include an interactive chatbot with faculty-specific knowledge, an interactive map, and an attendance list for faculty staff. These functionalities aim to provide quick and efficient access to information that would otherwise require a visit to the dean's office or other administrative departments.

### Main Features

1. **Chatbot:**
   - The chatbot welcomes users with a greeting message.
   - Users can chat with the bot, which categorizes each message as either a chat or a map request.
   - If categorized as a map request, the bot redirects the user to the map tab, showing the requested route.
   - If categorized as a chat request, the bot searches the FAQ database for relevant answers.
   - If a similar FAQ is found, the bot asks for confirmation before displaying the answer.
   - If no FAQ match is found, the bot uses a Large Language Model (LLM) for a response.

2. **Map:**
   - The interactive map allows users to find lecture halls and other important locations within the faculty building.
   - Users can highlight specific rooms and view detailed descriptions, including information about the person in charge.

3. **Attendance List:**
   - The attendance list shows all faculty staff members, including those in special roles such as the dean's office or the Laboratory of Computer System Operation (LESK).
   - Each entry includes the staff member's name, title, office number, and an availability indicator (green for present, red for absent).
   - A search bar at the top allows users to filter the list by name or title.

### Technologies and Tools

#### Frontend
- **.NET MAUI** and **C#**
- **XAML** for UI design
- **MVVM** (Model-View-ViewModel) design pattern

#### Libraries
- **Newtonsoft.Json** 13.0.3: JSON framework for .NET
- **Microsoft.NET.ILLink.Tasks** 8.0.5: MSBuild tasks for IL Linker
- **Microsoft.Maui.Controls.Compatibility** 8.0.7: Compatibility APIs for .NET MAUI
- **Microsoft.Maui.Controls** 8.0.7: Cross-platform framework for mobile and desktop apps
- **CommunityToolkit.Mvvm** 8.2.2: MVVM library with various helpers
- **CommunityToolkit.Maui** 7.0.1: Toolkit for common developer tasks in .NET MAUI
- **banditoth.MAUI.DeviceId** 1.0.0: Toolkit for device identification in .NET MAUI

#### Backend
- **PostgreSQL** for database management
- **.NET Core** for backend services
- **DeepL** for translation services
- **Vicuna** based on the **LLama** model for AI responses
- **Python** for various backend tasks
- **Qdrant** for vector similarity search
- **Hugging Face** and **LangChain** for advanced AI and NLP tasks

### Video Demonstrations
Below are video links showcasing the final product and its functionalities:

Authentication            |  FAQ                       | LLM                       | Employees
:-------------------------:|:-------------------------:|:-------------------------:|:-------------------------:
<img src="https://github.com/KamilFicerman/DepartmentChatbot/blob/main/gifs/VID_20240602204116.gif" width="200" height="500" />  |  <img src="https://github.com/KamilFicerman/DepartmentChatbot/blob/main/gifs/VID_20240602212114.gif" width="200" height="500" /> |  <img src="https://github.com/KamilFicerman/DepartmentChatbot/blob/main/gifs/VID_20240602212425.gif" width="200" height="500" /> |  <img src="https://github.com/KamilFicerman/DepartmentChatbot/blob/main/gifs/VID_20240602212650.gif" width="200" height="500" />
