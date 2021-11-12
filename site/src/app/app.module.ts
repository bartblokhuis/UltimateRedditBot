import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './components/shared/header/header.component';
import { FooterComponent } from './components/shared/footer/footer.component';
import { HomeComponent } from './components/home/home.component';
import { FeaturesComponent } from './components/home/features/features.component';
import { StatisticsComponent } from './components/home/statistics/statistics.component';
import { LandingComponent } from './components/home/landing/landing.component';
import { CommandsComponent } from './components/commands/commands.component';
import { SupportComponent } from './components/support/support.component';

import { CountToDirective } from './directives/count-to.directive';

@NgModule({
  declarations: [
    AppComponent,
    FooterComponent,
    HeaderComponent,
    HomeComponent,
    FeaturesComponent,
    StatisticsComponent,
    LandingComponent,
    CommandsComponent,
    SupportComponent,
    CountToDirective
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
