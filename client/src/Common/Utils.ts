import { User } from "./Interfaces/User";

export const formatUserDisplayName = (user: User): string => {
    return `${user.givenNames} ${user.familyName}`;
}