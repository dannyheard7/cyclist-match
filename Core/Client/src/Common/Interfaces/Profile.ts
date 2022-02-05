import Availability from '../Enums/Availability';
import CyclingType from '../Enums/CyclingType';
import { User } from './User';

export default interface Profile extends User {
    userDisplayName: string;
    locationName: string;
    cyclingTypes: Array<CyclingType>;
    availability: Array<Availability>;
    averageDistance: number;
    averageSpeed: number;
    distanceFromUserKM: number;
    userPicture: string | null;
}
