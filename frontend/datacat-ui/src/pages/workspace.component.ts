/**
 * Since applictaion is really an SPA, it only
 * requires one page. Other UI elements are
 * located in /widgets.
 */

import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';

import { SplitterModule } from 'primeng/splitter';
import { BreadcrumbModule } from 'primeng/breadcrumb';
import { TreeModule, TreeNodeSelectEvent } from 'primeng/tree';
import { SelectModule } from 'primeng/select';
import { ButtonModule } from 'primeng/button';
import { MenuItem, TreeNode } from 'primeng/api';
import { Location } from '@angular/common';
import { PathToActivityComponent } from '../widgets/common/path-to-activity.component';
import * as urls from '../entities/urls';

interface TreeMenuNodeData {
  url?: string;
}

@Component({
  standalone: true,
  selector: 'datacat-main-page',
  templateUrl: './workspace.component.html',
  styleUrl: './workspace.component.scss',
  imports: [
    RouterModule,
    SplitterModule,
    BreadcrumbModule,
    TreeModule,
    SelectModule,
    ButtonModule,
    PathToActivityComponent,
  ],
})
export class WorkspaceComponent {
  constructor(
    private router: Router,
    private location: Location,
  ) {}

  protected activityPathItems: MenuItem[] = [];
  protected readonly treeMenuRootNode: TreeNode<TreeMenuNodeData>[] = [
    {
      label: 'Namespace',
      children: [
        {
          type: 'url',
          icon: 'pi pi-megaphone',
          label: 'Alerts',
          data: {
            url: urls.ALERTS_EXPLORER_URL,
          },
        },
        {
          type: 'url',
          icon: 'pi pi-envelope',
          label: 'Notifications',
          data: {
            url: urls.NOTIFICATION_CHANNELS_URL,
          },
        },
        {
          type: 'url',
          icon: 'pi pi-table',
          label: 'Dashboards',
          data: {
            url: urls.DASHBOARDS_EXPLORER_URL,
          },
        },
        {
          type: 'url',
          icon: 'pi pi-crown',
          label: 'Admin',
          data: {
            url: urls.ADMIN_URL,
          },
        },
      ],
    },
  ];

  protected onTreeMenuNodeSelect(event: TreeNodeSelectEvent) {
    if (event.node.data && event.node.data.url) {
      this.router.navigate([event.node.data.url]);
    }
  }

  protected goToPreviousActivity() {
    this.location.back();
  }

  protected goToNextActivity() {
    this.location.forward();
  }

  protected openSettings() {
    this.router.navigate(['settings']);
  }
}
