import CyclingType from "../Enums/CyclingType";

export interface Profile {
    givenNames: string,
    familyName: string,
    placeName: string
    preferredCyclingTypes: CyclingType[]
    picture?: string,
    createdAt: Date
}