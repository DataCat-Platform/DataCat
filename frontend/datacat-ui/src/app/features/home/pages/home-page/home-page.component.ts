import { Component } from '@angular/core';

@Component({
  selector: 'app-home-page',
  standalone: true,
  imports: [],
  templateUrl: './home-page.component.html'
})
export class HomePageComponent {
  welcomeMessage = 'Welcome to the DataCat Observability System!';
  tutorials: string[] = [
    'Getting Started',
    'Creating Your First Dashboard',
    'Understanding Metrics',
    'Setting Up Alerts'
  ];
  helpfulLinks: string[] = [
    'Dashboard Management',
    'User Settings',
    'Documentation',
    'Support'
  ];
}
