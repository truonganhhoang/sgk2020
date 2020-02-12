/**
 * @author       Digitsensitive <digit.sensitivee@gmail.com>
 * @copyright    2019 Digitsensitive
 * @description  Super Mario Land
 * @license      Digitsensitive
 */

import "phaser";
import { BootScene } from "./scenes/boot-scene";
import { GameScene } from "./scenes/game-scene";
import { HUDScene } from "./scenes/hud-scene";
import { MenuScene } from "./scenes/menu-scene";

const config: Phaser.Types.Core.GameConfig = {
  width: 160,
  height: 144,
  zoom: 5,
  type: Phaser.AUTO,
  scale: {
    autoCenter: Phaser.Scale.CENTER_BOTH,
  },
  parent: "game",
  scene: [BootScene, MenuScene, HUDScene, GameScene],
  input: {
    keyboard: true
  },
  physics: {
    default: "arcade",
    arcade: {
      gravity: { y: 475 },
      debug: false
    }
  },
  backgroundColor: "#f8f8f8",
  render: { pixelArt: true, antialias: false }
};

export class SuperMarioLand extends Phaser.Game {
  constructor() {
    super(config);
  }
}
