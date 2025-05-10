import { Injectable } from '@angular/core';
import { Theme } from './select-theme.types';

@Injectable({
  providedIn: 'root',
})
export class ThemeSelectionService {
  public readonly themes: Theme[] = Object.values(Theme);
  private theme: Theme = Theme.DARK;

  private themeKey: string = 'app.theme';
  private darkThemeSelector: string = 'use-dark-theme';

  constructor() {
    this.loadSavedTheme();
  }

  public get currentTheme(): Theme {
    return this.theme;
  }

  public set currentTheme(theme: Theme) {
    this.theme = theme;

    const element = document.querySelector('html')!;
    switch (this.currentTheme) {
      case Theme.LIGHT: {
        element.classList.remove(this.darkThemeSelector);
        break;
      }
      case Theme.DARK: {
        element.classList.add(this.darkThemeSelector);
        break;
      }
      default: {
        throw new Error('Unknown theme');
      }
    }

    localStorage.setItem(this.themeKey, theme);
  }

  public loadSavedTheme() {
    const savedTheme = localStorage.getItem(this.themeKey);

    if (savedTheme === null) {
      return;
    }

    if (Object.values(Theme).includes(savedTheme as Theme)) {
      this.currentTheme = savedTheme as Theme;
    } else {
      localStorage.removeItem(this.themeKey);
    }
  }
}
