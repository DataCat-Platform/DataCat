import {Injectable} from '@angular/core';
import {MessageService} from "primeng/api";

@Injectable({
    providedIn: 'root'
})
export class ToastLoggerService {

    constructor(private messageService: MessageService) {
    }

    success(message: any): void {
        console.log(message);
        this.messageService.add({severity: 'success', summary: 'Success', detail: message});
    }

    info(message: any): void {
        console.log(message);
        this.messageService.add({severity: 'info', summary: 'Info', detail: message});
    }

    warn(message: any): void {
        console.warn(message);
        this.messageService.add({severity: 'warn', summary: 'Warning', detail: message});
    }

    error(message: any): void {
        console.error(message);
        this.messageService.add({severity: 'error', summary: 'Error', detail: message});
    }
}
