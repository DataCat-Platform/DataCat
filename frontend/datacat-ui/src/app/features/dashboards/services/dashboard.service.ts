import { Injectable } from '@angular/core';
import { HttpClient } from "@microsoft/signalr";
import { environment } from "../../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  private readonly baseUrl = environment.apiUrl;

  constructor(private readonly http: HttpClient) {
  }

  getDashboards(page: number, pageSize: number) {
    return this.http.get(`${this.baseUrl}/dashboards?page=${page}&pageSize=${pageSize}`);
  }

  searchDashboards(filter: string, page: number, pageSize: number) {
    return this.http.get(`${this.baseUrl}/dashboards/search?filter=${filter}&page=${page}&pageSize=${pageSize}`);
  }

  getDashboardById(id: string) {
    return this.http.get(`${this.baseUrl}/dashboards/${id}`);
  }
}

