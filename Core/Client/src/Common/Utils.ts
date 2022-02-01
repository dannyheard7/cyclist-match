export function formatMessageTimestamp(timestamp: Date): string {
    return new Intl.DateTimeFormat(undefined, {
        hour: 'numeric',
        minute: 'numeric',
        month: 'short',
        day: 'numeric',
    }).format(timestamp);
}
