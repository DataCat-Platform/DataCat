export function sanitizeMessage(message: string): string {
    try {
        if ((message.startsWith('"') && message.endsWith('"')) ||
            (message.startsWith('\"') && message.endsWith('\"'))) {
            message = message.slice(1, -1);
        }

        return JSON.parse(`"${message}"`);
    } catch {
        return message;
    }
}
