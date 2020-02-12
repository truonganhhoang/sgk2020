/**
 * @author       Digitsensitive <digit.sensitivee@gmail.com>
 * @copyright    2019 Digitsensitive
 * @description  Super Mario Land: Mario
 * @license      Digitsensitive
 */

export class Mario extends Phaser.GameObjects.Sprite {
  // variables
  private currentScene: Phaser.Scene;
  private marioSize: string;
  private acceleration: number;
  private isJumping: boolean;
  private isDying: boolean;
  private isVulnerable: boolean;
  private vulnerableCounter: number;

  // input
  private keys: Map<string, Phaser.Input.Keyboard.Key>;

  public getKeys(): Map<string, Phaser.Input.Keyboard.Key> {
    return this.keys;
  }

  constructor(params) {
    super(params.scene, params.x, params.y, params.key, params.frame);

    this.currentScene = params.scene;
    this.initSprite();
    this.currentScene.add.existing(this);
  }

  private initSprite() {
    // variables
    this.marioSize = this.currentScene.registry.get("marioSize");
    this.acceleration = 500;
    this.isJumping = false;
    this.isDying = false;
    this.isVulnerable = true;
    this.vulnerableCounter = 100;

    // sprite
    this.setOrigin(0.5, 0.5);
    this.setFlipX(false);

    // input
    this.keys = new Map([
      ["LEFT", this.addKey("LEFT")],
      ["RIGHT", this.addKey("RIGHT")],
      ["DOWN", this.addKey("DOWN")],
      ["JUMP", this.addKey("SPACE")]
    ]);

    // physics
    this.currentScene.physics.world.enable(this);
    this.adjustPhysicBodyToSmallSize();
    (<Phaser.Physics.Arcade.Body>this.body).maxVelocity.x = 50;
    (<Phaser.Physics.Arcade.Body>this.body).maxVelocity.y = 300;
  }

  private addKey(key: string): Phaser.Input.Keyboard.Key {
    return this.currentScene.input.keyboard.addKey(key);
  }

  update(): void {
    if (!this.isDying) {
      this.handleInput();
      this.handleAnimations();
    } else {
      this.setFrame(12);
      if (this.y > this.currentScene.sys.canvas.height) {
        this.currentScene.scene.stop("GameScene");
        this.currentScene.scene.stop("HUDScene");
        this.currentScene.scene.start("MenuScene");
      }
    }

    if (!this.isVulnerable) {
      if (this.vulnerableCounter > 0) {
        this.vulnerableCounter -= 1;
      } else {
        this.vulnerableCounter = 100;
        this.isVulnerable = true;
      }
    }
  }

  private handleInput() {
    if (this.y > this.currentScene.sys.canvas.height) {
      // mario fell into a hole
      this.isDying = true;
    }

    // evaluate if player is on the floor or on object
    // if neither of that, set the player to be jumping
    if (
      (<Phaser.Physics.Arcade.Body>this.body).onFloor() ||
      (<Phaser.Physics.Arcade.Body>this.body).touching.down ||
      (<Phaser.Physics.Arcade.Body>this.body).blocked.down
    ) {
      this.isJumping = false;
      //(<Phaser.Physics.Arcade.Body>this.body).setVelocityY(0);
    }

    // handle movements to left and right
    if (this.keys.get("RIGHT").isDown) {
      (<Phaser.Physics.Arcade.Body>this.body).setAccelerationX(this.acceleration);
      this.setFlipX(false);
    } else if (this.keys.get("LEFT").isDown) {
      (<Phaser.Physics.Arcade.Body>this.body).setAccelerationX(-this.acceleration);
      this.setFlipX(true);
    } else {
      (<Phaser.Physics.Arcade.Body>this.body).setVelocityX(0);
      (<Phaser.Physics.Arcade.Body>this.body).setAccelerationX(0);
    }

    // handle jumping
    if (this.keys.get("JUMP").isDown && !this.isJumping) {
      (<Phaser.Physics.Arcade.Body>this.body).setVelocityY(-180);
      this.isJumping = true;
    }
  }

  private handleAnimations(): void {
    if ((<Phaser.Physics.Arcade.Body>this.body).velocity.y !== 0) {
      // mario is jumping or falling
      this.anims.stop();
      if (this.marioSize === "small") {
        this.setFrame(4);
      } else {
        this.setFrame(10);
      }
    } else if ((<Phaser.Physics.Arcade.Body>this.body).velocity.x !== 0) {
      // mario is moving horizontal

      // check if mario is making a quick direction change
      if (
        ((<Phaser.Physics.Arcade.Body>this.body).velocity.x < 0 && (<Phaser.Physics.Arcade.Body>this.body).acceleration.x > 0) ||
        ((<Phaser.Physics.Arcade.Body>this.body).velocity.x > 0 && (<Phaser.Physics.Arcade.Body>this.body).acceleration.x < 0)
      ) {
        if (this.marioSize === "small") {
          this.setFrame(5);
        } else {
          this.setFrame(11);
        }
      }

      if ((<Phaser.Physics.Arcade.Body>this.body).velocity.x > 0) {
        this.anims.play(this.marioSize + "MarioWalk", true);
      } else {
        this.anims.play(this.marioSize + "MarioWalk", true);
      }
    } else {
      // mario is standing still
      this.anims.stop();
      if (this.marioSize === "small") {
        this.setFrame(0);
      } else {
        if (this.keys.get("DOWN").isDown) {
          this.setFrame(13);
        } else {
          this.setFrame(6);
        }
      }
    }
  }

  private growMario(): void {
    this.marioSize = "big";
    this.currentScene.registry.set("marioSize", "big");
    this.adjustPhysicBodyToBigSize();
  }

  private shrinkMario(): void {
    this.marioSize = "small";
    this.currentScene.registry.set("marioSize", "small");
    this.adjustPhysicBodyToSmallSize();
  }

  private adjustPhysicBodyToSmallSize(): void {
    (<Phaser.Physics.Arcade.Body>this.body).setSize(6, 12);
    (<Phaser.Physics.Arcade.Body>this.body).setOffset(6, 4);
  }

  private adjustPhysicBodyToBigSize(): void {
    (<Phaser.Physics.Arcade.Body>this.body).setSize(8, 16);
    (<Phaser.Physics.Arcade.Body>this.body).setOffset(4, 0);
  }

  private bounceUpAfterHitEnemyOnHead(): void {
    this.currentScene.add.tween({
      targets: this,
      props: { y: this.y - 5 },
      duration: 200,
      ease: "Power1",
      yoyo: true
    });
  }

  protected gotHit(): void {
    this.isVulnerable = false;
    if (this.marioSize === "big") {
      this.shrinkMario();
    } else {
      // mario is dying
      this.isDying = true;

      // sets acceleration, velocity and speed to zero
      // stop all animations
      (<Phaser.Physics.Arcade.Body>this.body).stop();
      this.anims.stop();

      // make last dead jump and turn off collision check
      (<Phaser.Physics.Arcade.Body>this.body).setVelocityY(-180);

      // (<Phaser.Physics.Arcade.Body>this.body).checkCollision.none did not work for me
      (<Phaser.Physics.Arcade.Body>this.body).checkCollision.up = false;
      (<Phaser.Physics.Arcade.Body>this.body).checkCollision.down = false;
      (<Phaser.Physics.Arcade.Body>this.body).checkCollision.left = false;
      (<Phaser.Physics.Arcade.Body>this.body).checkCollision.right = false;
    }
  }
}
