import {computed, Injectable, signal} from "@angular/core";
import {ApiService, GetMeResponse} from "./datacat-generated-client";
import {NamespaceService} from "./namespace.service";

@Injectable({
    providedIn: 'root'
})
export class UserService {
    private me = signal<GetMeResponse | null>(null);

    hasPermission = computed(() => {
        const current = this.namespaceService.currentNamespace();
        const claims = this.me()?.claims ?? [];
        return (permission: string) => {
            return claims.some(c =>
                c.namespaceId === current.id && c.role === permission
            );
        };
    });

    constructor(
        private apiService: ApiService,
        private namespaceService: NamespaceService
    ) {
        this.apiService.getApiV1UserGetMe().subscribe({
            next: user => this.me.set(user),
            error: err => console.error(err)
        });
    }
}
