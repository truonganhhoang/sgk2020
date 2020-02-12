import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { FlappyBird } from '../flappy-bird';
import { SuperMarioLand } from '../super-mario-land';

@Component({
  selector: 'app-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.scss']
})
export class GameComponent {
  name = '';
  id = '';
  game;

  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    this.id = this.route.snapshot.paramMap.get('id');
    switch (this.id) {
      case 'flappy-bird':
        this.name = "Flappy Bird";
        this.game = new FlappyBird();
        break;

      case 'super-mario-land':
        this.name = "Super Mario Land";
        this.game = new SuperMarioLand();
        break;

      case '???':
        this.name = "???";
        this.game = null;
        break;

      default:
        break;
    }
  }
}
