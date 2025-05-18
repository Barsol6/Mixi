# Mixi

**Mixi** is a self-hosted, minimalistic Blazor web application designed to assist with organizing and running tabletop role-playing games (TTRPGs). It offers tools for both Game Masters and players to streamline session prep, gameplay, and immersion.

## ✨ Features

- 🎭 Game Master and Player accounts
- 🎲 Dice roller
- 🧠 Name generator
- 📇 Character card support
- 🎵 Music player (local; Spotify and Tidal planned)
- 🧩 Modular and extensible architecture

## 🛠️ Tech Stack

- Blazor WebAssembly
- C#
- .NET
- Tailwind CSS

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Node.js & npm (for Tailwind CSS)

### Clone the repository

```bash
git clone https://github.com/Barsol6/mixi.git
cd mixi
```

### Install Tailwind CSS

1. Initialize Tailwind (if not already):

```bash
npm install -D tailwindcss postcss autoprefixer
npx tailwindcss init -p
```

2. Make sure `tailwind.config.js` includes the following:

```js
content: [
  "./**/*.razor",
  "./**/*.html"
]
```

3. Add Tailwind directives to your `wwwroot/css/app.css` or similar:

```css
@tailwind base;
@tailwind components;
@tailwind utilities;
```

4. Build Tailwind assets (you can also automate this via your build pipeline):

```bash
npx tailwindcss -i ./wwwroot/css/app.css -o ./wwwroot/css/output.css --watch
```

### Build and Run the App

```bash
dotnet run --project Mixi
```

Then open [http://localhost:5000](http://localhost:5000) (or whatever port it specifies).

> If it's a Blazor WebAssembly project, you can also run from Visual Studio with the browser set as launch target.


## 🧩 Planned Features

- Music playlists (Spotify/Tidal integration)
- Campaign/session manager
- Player dashboard

## 🤝 Contributing

Pull requests are welcome! To contribute:

1. Fork the project
2. Create your feature branch: `git checkout -b feature/my-feature`
3. Commit your changes: `git commit -m 'Add feature'`
4. Push to the branch: `git push origin feature/my-feature`
5. Open a pull request

## 📄 License

Licensed under the [GNU General Public License v3.0](LICENSE).