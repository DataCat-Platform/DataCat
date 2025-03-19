import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DrawerService {
  visible = signal<boolean>(false);
}
