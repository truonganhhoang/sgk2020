/**
 * @author       Digitsensitive <digit.sensitivee@gmail.com>
 * @copyright    2018 - 2019 digitsensitive
 * @description  Flappy Bird: Pipe
 * @license      Digitsensitive
 */

export class Pipe extends Phaser.GameObjects.Image {
  constructor(params) {
    super(params.scene, params.x, params.y, params.key, params.frame);

    // image
    this.setScale(3);
    this.setOrigin(0, 0);

    // physics
    this.scene.physics.world.enable(this);
    (<Phaser.Physics.Arcade.Body>this.body).allowGravity = false;
    (<Phaser.Physics.Arcade.Body>this.body).setVelocityX(-200);
    (<Phaser.Physics.Arcade.Body>this.body).setSize(20, 20);

    this.scene.add.existing(this);
  }
}
