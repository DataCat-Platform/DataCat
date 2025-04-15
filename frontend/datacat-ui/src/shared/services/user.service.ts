import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  getAvailableNamespaces(): string[] {
    return ['test namespace'];
  }
}
