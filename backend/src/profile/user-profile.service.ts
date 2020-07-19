import { Injectable } from '@nestjs/common';
import { InjectRepository } from '@nestjs/typeorm';
import { Repository } from 'typeorm';
import { UserProfile } from './UserProfile.entity';
import { UserProfileInput } from './UserProfile.input'
import { Point } from 'geojson';

@Injectable()
export class UserProfileService {
    constructor(
        @InjectRepository(UserProfile)
        private readonly userProfileRepository: Repository<UserProfile>,
    ) { }

    findById = (id: string) => this.userProfileRepository.findOne({ userId: id });

    findByUser = (user: User) => this.userProfileRepository.findOne({ userId: user.sub });

    async create(userProfileInput: UserProfileInput, user: User): Promise<UserProfile> {
        var exactLocation: Point = { type: "Point", coordinates: [userProfileInput.exactLocationLongitude, userProfileInput.exactLocationLatitude] }
        const userProfile = new UserProfile(user.sub, userProfileInput.name, userProfileInput.placeName, exactLocation, userProfileInput.preferredCyclingTypes);
        return await this.userProfileRepository.save(userProfile)
    }
}
