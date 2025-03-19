import { Component, OnInit } from '@angular/core';
import { MenuItem } from "primeng/api";
import { Button } from "primeng/button";
import { Drawer } from "primeng/drawer";

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [
    Button,
    Drawer
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent implements OnInit {
  visible = false;

  mainItems: MenuItem[] = null!;
  settingsItems: MenuItem[] = null!;

  ngOnInit() {
    this.mainItems = [
      {
        label: 'Home',
        icon: 'pi pi-home',
        routerLink: '/',
      },
      {
        label: 'Dashboards',
        icon: 'pi pi-chart-bar',
        routerLink: '/dashboards',
      },
      {
        label: 'Monitoring',
        icon: 'pi pi-chart-line',
        items: [
          {
            label: 'Metrics',
            icon: 'pi pi-heart-fill',
            routerLink: '/metrics',
          },
          {
            label: 'Logs',
            icon: 'pi pi-file',
            routerLink: '/logs',
          },
          {
            label: 'Traces',
            icon: 'pi pi-search',
            routerLink: '/traces',
          }
        ]
      },
      {
        label: 'Alerts',
        icon: 'pi pi-bell',
        items: [
          {
            label: 'Alerts',
            icon: 'pi pi-cog',
            routerLink: '/alerts',
          },
          {
            label: 'Active Alerts',
            icon: 'pi pi-exclamation-triangle',
            routerLink: '/active-alerts',
          }
        ]
      },
      {
        label: 'Server',
        icon: 'pi pi-server',
        items: [
          {
            label: 'Collector Configuration',
            icon: 'pi pi-sliders-h',
            routerLink: '/collector-configuration',
          },
          {
            label: 'Server Settings',
            icon: 'pi pi-server',
            routerLink: '/server-settings',
          }
        ]
      }
    ];

    this.settingsItems = [
      {
        label: 'Settings',
        icon: 'pi pi-cog',
        items: [
          {
            label: 'Configuration',
            icon: 'pi pi-bars',
            routerLink: '/settings/configuration',
          },
          {
            label: 'UI',
            icon: 'pi pi-map',
            routerLink: '/settings/ui',
          },
          {
            label: 'GitHub',
            icon: 'pi pi-github',
            routerLink: 'https://github.com/DataCat-Platform/DataCat',
          }
        ]
      }
    ]
  }
}
