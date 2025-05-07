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
                    window.location.href = `/api/v1/user/login-code-flow`;
                    // const alreadyTriedLogin = localStorage.getItem('login_attempted');
                    //
                    // if (!alreadyTriedLogin) {
                    //     localStorage.setItem('login_attempted', 'true');
                    //     window.location.href = `/api/v1/user/login-code-flow`;
                    // } else {
                    //     localStorage.removeItem('login_attempted'); // Сброс для будущих попыток
                    //     window.location.href = '/forbidden';
                    // }
                }
            }
        })
    );
};

export function getAccessTokenFromCookie(): string | null {
    const match = document.cookie.match(/(?:^| )access_token=([^;]+)/);
    return match ? match[1] : null;
}
