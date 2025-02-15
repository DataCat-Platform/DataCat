import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { environment } from "../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class RealtimeMetricsService {
  private hubConnection: signalR.HubConnection;
  private readonly baseUrl = environment.apiUrl;

  constructor() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${this.baseUrl}/datacat-metrics`)
      .build();
    this.hubConnection.on('ReceiveMessage', (user, message) => {
      console.log(`User: ${user}, Message: ${message}`);
    });
    this.hubConnection.start()
      .catch(err => console.error(err));
  }

  sendMessage(user: string, message: string): void {
    this.hubConnection.invoke('Send', user, message)
      .catch(err => console.error(err));
  }
}
