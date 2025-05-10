import {Injectable, signal} from "@angular/core";
import {ApiService, GetAvailableNamespaceResponse} from "./datacat-generated-client";
import {catchError, tap} from "rxjs";
import {CURRENT_NAMESPACE_KEY, getCurrentNamespaceFromLocalStorage} from "../utils/getCurrentNamespaceFromLocalStorage";

@Injectable({
    providedIn: 'root'
})
export class NamespaceService {
    readonly currentNamespace = signal<GetAvailableNamespaceResponse>(getCurrentNamespaceFromLocalStorage());
    readonly namespaces = signal<GetAvailableNamespaceResponse[]>([]);
    private readonly STORAGE_KEY = 'CURRENT_NAMESPACE';

    constructor(
        private apiService: ApiService,
    ) {
        this.apiService.getApiV1NamespaceGetAvailable().pipe(
            tap(data => {
                if (data) {
                    this.namespaces.set(data);
                }
            }),
            catchError(err => {
                return err;
            })
        ).subscribe();
    }

    setCurrentNamespace(ns: GetAvailableNamespaceResponse): void {
        localStorage.setItem(CURRENT_NAMESPACE_KEY, JSON.stringify(ns));
        this.currentNamespace.set(ns);
    }
}
