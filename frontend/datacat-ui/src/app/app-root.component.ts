import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SplitterModule } from 'primeng/splitter';
import { BreadcrumbModule } from 'primeng/breadcrumb';
import { MenuItem, TreeNode } from 'primeng/api';
import { TreeModule } from 'primeng/tree';
import { RouterModule } from '@angular/router';
import { SelectModule } from 'primeng/select';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterModule,
    SplitterModule,
    BreadcrumbModule,
    TreeModule,
    SelectModule,
  ],
  templateUrl: './app-root.component.html',
})
export class AppComponent implements OnInit {
  // Breadcrumb parameters.
  breadcrumbItems!: MenuItem[];
  breadcrumbHome!: MenuItem;

  treeMenuNodes!: TreeNode[];

  title = 'datacat-ui';

  constructor(private router: Router) {}

  ngOnInit() {
    this.breadcrumbItems = this.router.url
      .split('/')
      .filter((v) => v.length !== 0)
      .map<MenuItem>((URLSegment) => {
        return {
          label: URLSegment,
        };
      });
    this.breadcrumbHome = { icon: 'pi pi-home', routerLink: '/' };
    this.treeMenuNodes = [
      {
        label: 'Namespace',
        selectable: false,
        children: [
          {
            label: 'Manage members',
            icon: 'pi pi-users',
            type: 'url',
          },
          {
            label: 'Dashboards',
            icon: 'pi pi-th-large',
            type: 'url',
            data: '/dashboards',
          },
        ],
      },
      {
        label: 'Alerting',
        selectable: false,
        children: [
          {
            icon: 'pi pi-envelope',
            label: 'Channels',
            type: 'url',
            data: '/alerting/channels',
          },
          {
            icon: 'pi pi-bell',
            label: 'Alerts',
            type: 'url',
            data: '/alerting/alerts',
          },
        ],
      },
      {
        label: 'Settings',
        selectable: false,
        children: [
          {
            icon: 'pi pi-cog',
            label: 'UI',
            type: 'url',
            data: '/settings',
          },
        ],
      },
    ];
  }
}
