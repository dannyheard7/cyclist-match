import { AccountCircle, Menu as MenuIcon } from '@mui/icons-material';
import {
    AppBar,
    Box,
    Divider,
    Grid,
    Hidden,
    IconButton,
    Link,
    ListItemText,
    Menu,
    MenuItem,
    Toolbar,
    Typography,
} from '@mui/material';
import { styled } from '@mui/material/styles';
import React, { Fragment, useRef, useState } from 'react';
import { Link as RouterLink, useNavigate } from 'react-router-dom';
import { AuthenticatedState, useAuthentication } from '../Authentication/AuthWrapper';
import ConversationsIcon from '../Conversations/ConversationsIcon';
import AppDrawer, { DRAWER_WIDTH } from './AppDrawer';
const PREFIX = 'AppMenu';

const classes = {
    root: `${PREFIX}-root`,
    appBar: `${PREFIX}-appBar`,
    appBarShift: `${PREFIX}-appBarShift`,
    menuButton: `${PREFIX}-menuButton`,
    hide: `${PREFIX}-hide`,
    title: `${PREFIX}-title`,
    drawer: `${PREFIX}-drawer`,
    drawerPaper: `${PREFIX}-drawerPaper`,
    drawerHeader: `${PREFIX}-drawerHeader`,
    grow: `${PREFIX}-grow`,
    padding: `${PREFIX}-padding`,
};

const StyledAppBar = styled(AppBar, {
    shouldForwardProp: (prop) => !prop.toString().startsWith('$'),
})<{ $drawerOpen: boolean }>(({ theme, $drawerOpen }) => ({
    transition: theme.transitions.create(['margin', 'width'], {
        easing: theme.transitions.easing.sharp,
        duration: theme.transitions.duration.leavingScreen,
    }),
    ...($drawerOpen && {
        width: `calc(100% - ${DRAWER_WIDTH}px)`,
        marginLeft: DRAWER_WIDTH,
        transition: theme.transitions.create(['margin', 'width'], {
            easing: theme.transitions.easing.easeOut,
            duration: theme.transitions.duration.enteringScreen,
        }),
    }),
}));

const StyledMenuIcon = styled(IconButton, {
    shouldForwardProp: (prop) => !prop.toString().startsWith('$'),
})<{ $drawerOpen: boolean }>(({ theme, $drawerOpen }) => ({
    marginRight: theme.spacing(2),
    ...($drawerOpen && {
        display: 'none',
    }),
}));

const AppMenuRight: React.FC<{ authState: AuthenticatedState }> = ({
    authState: { appProfileExists, oidcProfile, signOut },
}) => {
    const [userMenuOpen, setUserMenuOpen] = useState<boolean>(false);
    const anchorEl = useRef(null);
    const push = useNavigate();

    return (
        <Fragment>
            {appProfileExists && (
                <Box sx={{ paddingx: 0.5 }}>
                    <IconButton color="inherit" onClick={() => push('/conversations')} size="large">
                        <ConversationsIcon />
                    </IconButton>
                </Box>
            )}
            <Box sx={{ paddingx: 0.5 }}>
                <IconButton
                    aria-label="account of current user"
                    aria-controls="menu-appbar"
                    aria-haspopup="true"
                    onClick={() => setUserMenuOpen(!userMenuOpen)}
                    color="inherit"
                    ref={anchorEl}
                    size="large"
                >
                    <AccountCircle />
                </IconButton>
            </Box>
            <Menu
                id="menu-appbar"
                anchorEl={anchorEl.current}
                open={userMenuOpen}
                onClose={() => setUserMenuOpen(false)}
            >
                <MenuItem disabled>
                    <Typography>
                        {oidcProfile.given_name} {oidcProfile.family_name}
                    </Typography>
                </MenuItem>
                <Divider sx={{ marginx: 1, marginy: 0.5 }} />
                <MenuItem component={RouterLink} to="/account" onClick={() => setUserMenuOpen(false)}>
                    Account
                </MenuItem>
                <MenuItem onClick={signOut}> Logout</MenuItem>
            </Menu>
        </Fragment>
    );
};

const AppMenu: React.FC = () => {
    const authState = useAuthentication();

    const [drawerOpen, setDrawerOpen] = useState<boolean>(false);

    return (
        <div>
            <AppDrawer open={drawerOpen} onClose={() => setDrawerOpen(false)} />
            <StyledAppBar position="fixed" $drawerOpen={drawerOpen}>
                <Toolbar>
                    <Grid container direction="row" justifyContent="space-between">
                        <Grid item container direction="row" xs={4} alignItems="center">
                            <StyledMenuIcon
                                edge="start"
                                $drawerOpen={drawerOpen}
                                color="inherit"
                                aria-label="menu"
                                onClick={() => setDrawerOpen(true)}
                                size="large"
                            >
                                <MenuIcon />
                            </StyledMenuIcon>

                            <Hidden smDown>
                                <Typography variant="h6">
                                    <Link component={RouterLink} to="/" style={{ color: 'white' }} underline="hover">
                                        BuddyUp!
                                    </Link>
                                </Typography>
                            </Hidden>
                        </Grid>
                        <Grid item container direction="row" xs={8} alignItems="center" justifyContent="end">
                            {authState.isLoggedIn === true && <AppMenuRight authState={authState} />}
                            {authState.isLoggedIn === false && (
                                <div style={{ marginLeft: 'auto', cursor: 'pointer' }}>
                                    <ListItemText onClick={authState.signIn}>Login</ListItemText>
                                </div>
                            )}
                        </Grid>
                    </Grid>
                </Toolbar>
            </StyledAppBar>
        </div>
    );
};

export default AppMenu;
