import { User } from "./Interfaces/User";

export const formatUserDisplayName = (user: User): string => {
    return `${user.givenNames} ${user.familyName}`;
}

export function formatMessageTimestamp(timestamp: Date): string {
    return new Intl.DateTimeFormat(undefined, { hour: 'numeric', minute: 'numeric', month: 'short', day: 'numeric' }).format(timestamp)
}