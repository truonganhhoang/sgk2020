/**
 * @author       Digitsensitive <digit.sensitivee@gmail.com>
 * @copyright    2019 Digitsensitive
 * @description  Super Mario Land: Goomba
 * @license      Digitsensitive
 */

import { Enemy } from "./enemy";

export class Goomba extends Enemy {
  constructor(params) {
    super(params);
    this.speed = -20;
    this.dyingScoreValue = 100;
  }

  update(): void {
    if (!this.isDying) {
      if (this.isActivated) {
        // goomba is still alive
        // add speed to velocity x
        (<Phaser.Physics.Arcade.Body>this.body).setVelocityX(this.speed);

        // if goomba is moving into obstacle from map layer, turn
        if ((<Phaser.Physics.Arcade.Body>this.body).blocked.right || (<Phaser.Physics.Arcade.Body>this.body).blocked.left) {
          this.speed = -this.speed;
          (<Phaser.Physics.Arcade.Body>this.body).velocity.x = this.speed;
        }

        // apply walk animation
        this.anims.play("goombaWalk", true);
      } else {
        if (
          Phaser.Geom.Intersects.RectangleToRectangle(
            this.getBounds(),
            this.currentScene.cameras.main.worldView
          )
        ) {
          this.isActivated = true;
        }
      }
    } else {
      // goomba is dying, so stop animation, make velocity 0 and do not check collisions anymore
      this.anims.stop();
      (<Phaser.Physics.Arcade.Body>this.body).setVelocity(0, 0);
      (<Phaser.Physics.Arcade.Body>this.body).checkCollision.none = true;
    }
  }

  protected gotHitOnHead(): void {
    this.isDying = true;
    this.setFrame(2);
    this.showAndAddScore();
  }

  protected gotHitFromBulletOrMarioHasStar(): void {
    this.isDying = true;
    (<Phaser.Physics.Arcade.Body>this.body).setVelocityX(20);
    (<Phaser.Physics.Arcade.Body>this.body).setVelocityY(-20);
    this.setFlipY(true);
  }

  protected isDead(): void {
    this.destroy();
  }
}
