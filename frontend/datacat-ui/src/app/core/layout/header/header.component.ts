import { Component, OnInit } from '@angular/core';
import { MenuItem } from "primeng/api";
import { Menubar } from "primeng/menubar";
import { Popover } from "primeng/popover";
import { Button } from "primeng/button";
import { NgForOf } from "@angular/common";

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [
    Menubar,
    Popover,
    Button,
    NgForOf
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent implements OnInit {
  mainItems: MenuItem[] = null!;
  settingsItems: MenuItem[] = null!;

  ngOnInit() {
    this.mainItems = [
      {
        label: 'Home',
        icon: 'pi pi-home'
      },
      {
        label: 'Dashboards',
        icon: 'pi pi-chart-bar'
      },
      {
        label: 'Monitoring',
        icon: 'pi pi-chart-line',
        items: [
          {
            label: 'Metrics',
            icon: 'pi pi-heart-fill'
          },
          {
            label: 'Logs',
            icon: 'pi pi-file'
          },
          {
            label: 'Traces',
            icon: 'pi pi-search'
          }
        ]
      },
      {
        label: 'Alerts',
        icon: 'pi pi-bell',
        items: [
          {
            label: 'Alert Configuration',
            icon: 'pi pi-cog'
          },
          {
            label: 'Active Alerts',
            icon: 'pi pi-exclamation-triangle'
          }
        ]
      },
      {
        label: 'Server',
        icon: 'pi pi-server',
        items: [
          {
            label: 'Collector Configuration',
            icon: 'pi pi-sliders-h'
          },
          {
            label: 'Server Settings',
            icon: 'pi pi-server'
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
            icon: 'pi pi-bars'
          },
          {
            label: 'UI',
            icon: 'pi pi-map'
          },
          {
            label: 'GitHub',
            icon: 'pi pi-github'
          }
        ]
      }
    ]
  }
}
