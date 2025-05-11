import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class NamespaceService {
  private _currentNamespaceId?: string;

  public get currentNamespaceId() {
    return this._currentNamespaceId;
  }

  public set currentNamespaceId(id: string | undefined) {
    if (id) {
      this._currentNamespaceId = id;
    }
  }

  public getCurrentNamespace() {}
}
