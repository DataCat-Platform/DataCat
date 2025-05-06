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
                    const alreadyRedirected = localStorage.getItem('login_redirected');

                    if (alreadyRedirected) {
                        window.location.href = "/forbidden";
                        localStorage.removeItem('login_redirected'); // todo: think about how to handle double auth 401/403 case
                    } else {
                        localStorage.setItem("login_redirected", "true");
                        const version = "v1";
                        window.location.href = `/api/${version}/user/login-code-flow`;
                    }
                }
            }
        })
    );
};

function getAccessTokenFromCookie(): string | null {
    const match = document.cookie.match(/(?:^| )access_token=([^;]+)/);
    return match ? match[1] : null;
}
