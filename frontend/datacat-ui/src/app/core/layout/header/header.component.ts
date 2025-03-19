import { Component } from '@angular/core';
import { MenuItem } from "primeng/api";
import { Drawer } from "primeng/drawer";
import { DrawerService } from "../../services/drawer.service";
import { PanelMenu } from "primeng/panelmenu";
import { NgClass, NgOptimizedImage } from "@angular/common";
import { RouterLink } from "@angular/router";
import { Tooltip } from "primeng/tooltip";
import { Divider } from "primeng/divider";

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [
    Drawer,
    PanelMenu,
    NgOptimizedImage,
    NgClass,
    RouterLink,
    Tooltip,
    Divider,
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {
  visible: boolean;
  mainItems: MenuItem[] = [
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
          icon: 'pi pi-heart',
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
    },
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
          url: 'https://github.com/DataCat-Platform/DataCat',
        }
      ]
    }
  ];
  subItems: MenuItem[] = [
    {
      label: 'Home',
      tooltip: 'Home page',
      icon: 'pi pi-home',
      routerLink: '/',
    },
    {
      label: 'Dashboards',
      tooltip: 'Go to Dashboards',
      icon: 'pi pi-chart-bar',
      routerLink: '/dashboards',
    },
    {
      label: 'divider',
      routerLink: '1',
    },
    {
      label: 'Metrics',
      tooltip: 'Watch metrics',
      icon: 'pi pi-heart',
      routerLink: '/metrics',
    },
    {
      label: 'Logs',
      tooltip: 'Watch logs',
      icon: 'pi pi-file',
      routerLink: '/logs',
    },
    {
      label: 'Traces',
      tooltip: 'Watch traces',
      icon: 'pi pi-search',
      routerLink: '/traces',
    },
    {
      label: 'Alerts',
      tooltip: 'Watch alerts',
      icon: 'pi pi-bolt',
      routerLink: '/alerts',
    },
    {
      label: 'divider',
      routerLink: '2',
    },
    {
      label: 'Server Settings',
      tooltip: 'Server Settings',
      icon: 'pi pi-server',
      routerLink: '/server-settings',
    },
    {
      label: 'UI',
      tooltip: 'UI settings',
      icon: 'pi pi-map',
      routerLink: '/settings/ui',
    }
  ];

  constructor(
    public drawerService: DrawerService) {
    this.visible = drawerService.visible();
  }

  onVisibleChange(visible: boolean) {
    this.visible = visible;
    this.drawerService.visible.set(visible);
  }
}
