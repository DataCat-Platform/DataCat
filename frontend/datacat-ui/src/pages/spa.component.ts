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

interface TreeMenuNodeData {
  url?: string;
}

@Component({
  standalone: true,
  selector: 'datacat-main-page',
  templateUrl: './spa.component.html',
  styleUrl: './spa.component.scss',
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
export class MainPageComponent {
  constructor(
    private _router: Router,
    private _location: Location,
  ) {}

  protected activityPathItems: MenuItem[] = [];
  protected readonly treeMenuRootNode: TreeNode<TreeMenuNodeData>[] = [
    {
      type: 'url',
      label: 'Alerts',
      data: {
        url: '/alerting/alerts',
      },
    },
    {
      label: 'Notification Channels',
    },
    {
      label: 'Dashboards',
    },
    {
      label: 'Plugins',
    },
  ];

  protected onTreeMenuNodeSelect(event: TreeNodeSelectEvent) {
    if (event.node.data && event.node.data.url) {
      this._router.navigate([event.node.data.url]);
    }
  }

  protected goToPreviousActivity() {
    this._location.back();
  }

  protected goToNextActivity() {
    this._location.forward();
  }

  protected openSettings() {
    this._router.navigate(['settings']);
  }
}
