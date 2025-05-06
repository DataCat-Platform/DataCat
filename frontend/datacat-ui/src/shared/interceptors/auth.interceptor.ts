import {HttpInterceptorFn} from "@angular/common/http";
import {tap} from "rxjs";

export const authInterceptor: HttpInterceptorFn = (req, next) => {
    const token = getAccessTokenFromCookie();

    const authReq = token
        ? req.clone({
            setHeaders: {
                Authorization: `Bearer ${token}`,
            },
        })
        : req;

    return next(authReq).pipe(
        tap({
            next: () => {
                // todo: ...
            },
            error: (error) => {
                if (error.status === 401) {
                    const loginAttempted = localStorage.getItem('login_attempted');

                    if (!loginAttempted) {
                        localStorage.setItem('login_attempted', 'true');
                        const version = 'v1';
                        window.location.href = `/api/${version}/user/login-code-flow`;
                    } else {
                        window.location.href = '/forbidden';
                    }
                }
            }
        })
    );
};

export function getAccessTokenFromCookie(): string | null {
    const match = document.cookie.match(/(?:^| )access_token=([^;]+)/);
    return match ? match[1] : null;
}
