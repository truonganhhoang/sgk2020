# Games

Dự án sử dụng [Angular CLI](https://github.com/angular/angular-cli) version 8.3.23.

## Cài đặt

thực thi lênh `npm install` để cài đặt thư viện `node_modules`.

## Chạy

thực thi lệnh `npm start` để bắt đầu chạy.

## Hướng dẫn tạo game

Có 2 game mẫu có thể tham khảo là `flappy-bird` và `super-mario-land`.

Tạo thư mục game tại `src/app`. Tên thư mục có thể là tên nhóm hoặc tên game, file khởi tạo game có tên `index.ts`.

Trong file `src/app/_home/home.component.html`, thêm đoạn code với tên thư mục game và tên game, mục đích để tạo tuyến đường url truy cập vào game

VD:
```html
<ul>
  ...
  <li>
    <a routerLink="/game/flappy-bird">Flappy Bird</a>
  </li>
  ...
</ul>
```

Trong file `src/app/_game_/game.component.ts`, thêm đoạn code với tên thư mục game, tên game và khởi tạo đối tượng game, mục đích để khởi tạo đối tượng game

VD:
```ts
switch (this.id) {
  ...
  case 'flappy-bird':
    this.name = "Flappy Bird";
    this.game = new FlappyBird();
    break;
  ...
}
```

Trong file `angular.json`, thêm đường dẫn chứa tài nguyên của game, mục đích để khai báo thư mục tài nguyên của game.

VD:
```json
{
  ...
  "architect": {
    "build": {
      "builder": "@angular-devkit/build-angular:browser",
      "options": {
        ...
        "assets": [
          "src/app/flappy-bird/assets",
          ...
        ],
        ...
      }
      ...
    }
    ...
  }
  ...
}
```

Nhớ viết `README.md` trong thư mục game để giới thiệu về game nhé!
