import { Component } from '@angular/core';
import { NgClass, NgIf } from "@angular/common";
import { Button } from "primeng/button";
import { Checkbox } from "primeng/checkbox";
import { PrimeTemplate } from "primeng/api";
import { FormsModule } from "@angular/forms";
import { TableModule } from "primeng/table";
import { DropdownModule } from "primeng/dropdown";
import { InputText } from "primeng/inputtext";
import { InputIcon } from "primeng/inputicon";
import { IconField } from "primeng/iconfield";
import { Tooltip } from "primeng/tooltip";

interface DashboardItem {
  name: string;
  author: string;
  teams: string;
  modified: string;
  popularity: number;
  checked?: boolean; // для чекбокса
}

@Component({
  selector: 'app-dashboard-list',
  standalone: true,
  imports: [
    NgClass,
    Button,
    Checkbox,
    PrimeTemplate,
    FormsModule,
    TableModule,
    DropdownModule,
    NgIf,
    InputText,
    InputIcon,
    IconField,
    Tooltip
  ],
  templateUrl: './dashboard-list.component.html'
})
export class DashboardListComponent {
  showControls = true;
  searchValue: string = '';
  isSearchLoading = false;

  // Пример опций для p-dropdown
  teamOptions = [
    {label: 'Namespace A', value: 'Namespace A'},
    {label: 'Namespace B', value: 'Namespace B'},
    {label: 'Namespace C', value: 'Namespace C'},
    {label: 'Namespace D', value: 'Namespace D'}
  ];
  selectedTeam: string | null = null;

  // Пример данных для p-table
  dashboardItems: DashboardItem[] = [
    {
      name: 'Ducking Audit Tail Overview',
      author: 'John Doe',
      teams: 'Team A',
      modified: 'Feb 12, 2025',
      popularity: 87
    },
    {
      name: 'Recent Counts',
      author: 'Jane Smith',
      teams: 'Team B',
      modified: 'Feb 15, 2025',
      popularity: 45
    },
    {
      name: 'Monitor Notifications Overview',
      author: 'Adam West',
      teams: 'Team A, Team C',
      modified: 'Mar 01, 2025',
      popularity: 65
    },
    {
      name: 'Reliability Overview by Services',
      author: 'Sarah Connor',
      teams: 'Team D',
      modified: 'Mar 10, 2025',
      popularity: 120
    },
    {
      name: 'Reliability Overview by Team',
      author: 'Ellen Ripley',
      teams: 'Team C',
      modified: 'Mar 14, 2025',
      popularity: 98
    },
    {
      name: 'System - Disk I/O',
      author: 'John Doe',
      teams: 'Team B',
      modified: 'Mar 16, 2025',
      popularity: 56
    },
    {
      name: 'System - Metrics',
      author: 'Jane Smith',
      teams: 'Team C',
      modified: 'Mar 18, 2025',
      popularity: 80
    },
    {
      name: 'System - Networking',
      author: 'Adam West',
      teams: 'Team A',
      modified: 'Mar 18, 2025',
      popularity: 74
    }
  ];

  toggleControls() {
    this.showControls = !this.showControls;
  }

  onSearch(value: string) {
    console.log('Search:', value);
    this.isSearchLoading = true;
    // Реализуйте логику поиска
  }

  newDashboard() {
    console.log('Create new dashboard');
    // Логика создания нового дашборда
  }

  editTeams() {
    console.log('Edit Teams clicked');
  }

  addTo() {
    console.log('Add to clicked');
  }

  deleteDashboards() {
    console.log('Delete clicked');
  }

  onMoreClick(item: DashboardItem) {
    console.log('More info:', item);
  }
}
