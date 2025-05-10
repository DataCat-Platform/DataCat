import {HttpInterceptorFn} from "@angular/common/http";
import {getCurrentNamespaceFromLocalStorage} from "../utils/getCurrentNamespaceFromLocalStorage";

export const namespaceInterceptor: HttpInterceptorFn = (req, next) => {
    const currentNamespace = getCurrentNamespaceFromLocalStorage();

    const apiReq = req.clone({
        setHeaders: {
            'X-Namespace': currentNamespace.id!
        }
    });

    return next(apiReq);
};
