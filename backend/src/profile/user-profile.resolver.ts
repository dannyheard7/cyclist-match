import { UseGuards } from '@nestjs/common';
import { Args, ID, Mutation, Query, Resolver } from '@nestjs/graphql';
import { GqlAuthGuard } from '../authz/auth.guard';
import { CurrentUser } from '../authz/current.user.decorator';
import { UserProfileService } from './user-profile.service';
import { UserProfile } from './UserProfile.entity';
import { UserProfileInput } from './UserProfile.input';


@Resolver(of => UserProfile)
export class UserProfileResolver {
    constructor(
        private readonly userProfileService: UserProfileService
    ) { }

    @UseGuards(GqlAuthGuard)
    @Query(() => UserProfile)
    async profile(
        @Args({ name: 'id', type: () => ID }) userId: string,
        @CurrentUser() user: User,
    ) {
        return await this.userProfileService.findByUser(user);
    }

    @UseGuards(GqlAuthGuard)
    @Mutation(() => UserProfile)
    async createUserProfile(
        @Args({ name: 'userProfile', type: () => UserProfileInput }) userProfileInput: UserProfileInput,
        @CurrentUser() user: User,
    ) {
        return await this.userProfileService.create(userProfileInput, user);
    }
}
