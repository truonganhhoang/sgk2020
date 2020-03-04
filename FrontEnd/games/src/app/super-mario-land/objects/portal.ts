/**
 * @author       Digitsensitive <digit.sensitivee@gmail.com>
 * @copyright    2019 Digitsensitive
 * @description  Super Mario Land: Portal
 * @license      Digitsensitive
 */

export class Portal extends Phaser.GameObjects.Zone {
  // variables
  private currentScene: Phaser.Scene;
  private portalDestinationForMario: any;

  public getPortalDestination(): {} {
    return this.portalDestinationForMario;
  }

  constructor(params) {
    super(params.scene, params.x, params.y, params.width, params.height);

    // variables
    this.currentScene = params.scene;
    this.portalDestinationForMario = params.spawn;
    this.initZone();
    this.currentScene.add.existing(this);
  }

  private initZone() {
    this.setOrigin(0, 0);

    // physics
    this.currentScene.physics.world.enable(this);
    (<Phaser.Physics.Arcade.Body>this.body).setSize(this.height, this.width);
    (<Phaser.Physics.Arcade.Body>this.body).setOffset(0, 0);
    (<Phaser.Physics.Arcade.Body>this.body).setAllowGravity(false);
    (<Phaser.Physics.Arcade.Body>this.body).setImmovable(true);
  }

  update(): void {}
}
