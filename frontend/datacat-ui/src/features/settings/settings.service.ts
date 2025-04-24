import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class SettingsService {
  readonly allThemes: string[] = ['light', 'dark'];
  private _currentTheme: string = this.allThemes[0];

  get currentTheme(): string {
    return this._currentTheme;
  }

  set currentTheme(theme: string) {
    this._currentTheme = theme;

    const element = document.querySelector('html')!;

    switch (this._currentTheme) {
      case 'light': {
        element.classList.remove('use-dark-theme');
        break;
      }
      case 'dark': {
        element.classList.add('use-dark-theme');
        break;
      }
      default: {
        throw new Error('Unknown theme');
      }
    }
  }
}
