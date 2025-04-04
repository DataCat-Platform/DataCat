import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from "./core/layout/header/header.component";
import { DrawerService } from "./core/services/drawer.service";
import { ApiService } from "./core/services/datacat-generated-client";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent],
  providers: [DrawerService, ApiService],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'datacat-ui';

  constructor(
    public drawerService: DrawerService,
    private apiServce: ApiService) {
  }
}

