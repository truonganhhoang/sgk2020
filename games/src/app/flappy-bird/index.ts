/**
 * @author       Digitsensitive <digit.sensitivee@gmail.com>
 * @copyright    2018 - 2019 digitsensitive
 * @description  Flappy Bird: Game
 * @license      Digitsensitive
 */

import "phaser";
import { BootScene } from "./scenes/boot-scene";
import { GameScene } from "./scenes/game-scene";
import { MainMenuScene } from "./scenes/main-menu-scene";

const config: Phaser.Types.Core.GameConfig = {
  width: 390,
  height: 600,
  type: Phaser.AUTO,
  scale: {
    autoCenter: Phaser.Scale.CENTER_BOTH,
  },
  parent: "game",
  scene: [BootScene, MainMenuScene, GameScene],
  input: {
    keyboard: true
  },
  physics: {
    default: "arcade",
    arcade: {
      gravity: { y: 300 }
    }
  },
  backgroundColor: "#98d687",
  render: { pixelArt: true, antialias: false }
};

export class FlappyBird extends Phaser.Game {
  constructor() {
    super(config);
  }
}
