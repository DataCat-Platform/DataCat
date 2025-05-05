import { Injectable } from '@angular/core';
import { Theme } from './select-theme.types';

@Injectable({
  providedIn: 'root',
})
export class ThemeSelectionService {
  private theme: Theme = Theme.LIGHT;
  public readonly themes: Theme[] = Object.values(Theme);

  public set currentTheme(theme: Theme) {
    this.theme = theme;

    const element = document.querySelector('html')!;
    switch (this.currentTheme) {
      case Theme.LIGHT: {
        element.classList.remove('use-dark-theme');
        break;
      }
      case Theme.DARK: {
        element.classList.add('use-dark-theme');
        break;
      }
      default: {
        throw new Error('Unknown theme');
      }
    }
  }

  public get currentTheme(): Theme {
    return this.theme;
  }

  constructor() {
    this.loadSavedTheme();
  }

  private loadSavedTheme() {
    const savedTheme = localStorage.getItem('app.theme');

    if (savedTheme === null) {
      return;
    }

    if (savedTheme in Theme) {
      this.currentTheme = savedTheme as Theme;
    } else {
      localStorage.removeItem('app.theme');
    }
  }
}
