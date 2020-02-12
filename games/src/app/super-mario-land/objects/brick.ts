/**
 * @author       Digitsensitive <digit.sensitivee@gmail.com>
 * @copyright    2019 Digitsensitive
 * @description  Super Mario Land: Brick
 * @license      Digitsensitive
 */

export class Brick extends Phaser.GameObjects.Sprite {
  // variables
  private currentScene: Phaser.Scene;
  protected destroyingValue: number;

  constructor(params) {
    super(params.scene, params.x, params.y, params.key, params.frame);

    // variables
    this.currentScene = params.scene;
    this.destroyingValue = params.value;
    this.initSprite();
    this.currentScene.add.existing(this);
  }

  private initSprite() {
    // sprite
    this.setOrigin(0, 0);
    this.setFrame(0);

    // physics
    this.currentScene.physics.world.enable(this);
    (<Phaser.Physics.Arcade.Body>this.body).setSize(8, 8);
    (<Phaser.Physics.Arcade.Body>this.body).setAllowGravity(false);
    (<Phaser.Physics.Arcade.Body>this.body).setImmovable(true);
  }

  update(): void {
    if ((<Phaser.Physics.Arcade.Body>this.body).touching.down) {
      // something touches the downside of the brick: probably mario?
      for (let i = -2; i < 2; i++) {
        // create smaller bricks
        let brick = this.currentScene.add
          .sprite(this.x, this.y, "brick")
          .setOrigin(0, 0)
          .setDisplaySize(4, 4);
        this.currentScene.physics.world.enable(brick);
        (<Phaser.Physics.Arcade.Body>brick.body).setVelocity(40 * i, -40 * i);
        (<Phaser.Physics.Arcade.Body>brick.body).setSize(4, 4);
      }

      // destroy brick
      this.destroy();

      // add some score for killing the brick
      this.currentScene.registry.values.score += this.destroyingValue;
      this.currentScene.events.emit("scoreChanged");
    }
  }
}
