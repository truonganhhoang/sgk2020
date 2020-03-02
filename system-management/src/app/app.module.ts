import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { SideNavOuterToolbarModule, SideNavInnerToolbarModule, SingleCardModule } from './layouts';
import { FooterModule, LoginFormModule } from './shared/components';
import { AuthService, ScreenService, AppInfoService } from './shared/services';
import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
// import { BaseComponent } from './base/base-component/base-component.component';
import { BasePopupComponent } from './base/base-popup/base-popup.component';
import { DirectiveModule } from './directive/directive.module';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    SideNavOuterToolbarModule,
    SideNavInnerToolbarModule,
    SingleCardModule,
    FooterModule,
    LoginFormModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    DirectiveModule
  ],
  providers: [AuthService, ScreenService, AppInfoService],
  bootstrap: [AppComponent]
})
export class AppModule { }
