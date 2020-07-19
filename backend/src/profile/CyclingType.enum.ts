import { registerEnumType } from "@nestjs/graphql";

export enum CyclingType {
    ROAD_BIKE,
    MOUNTAIN_BIKE,
    CYCLO_CROSS
}


registerEnumType(CyclingType, {
    name: 'CyclingType',
});