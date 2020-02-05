
import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { GameComponent } from './_game/game.component';
import { HomeComponent } from './_home/home.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'game/:id', component: GameComponent },
];

export const routing: ModuleWithProviders = RouterModule.forRoot(routes);
