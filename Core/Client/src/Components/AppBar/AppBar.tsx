import {
    AppBar,
    Divider,
    IconButton,
    ListItemText,
    Menu,
    MenuItem,
    Toolbar,
    Typography,
    useTheme,
    Link,
} from '@material-ui/core';
import { AccountCircle, Menu as MenuIcon } from '@material-ui/icons';
import classNames from 'classnames';
import React, { Fragment, useRef, useState } from 'react';
import { Link as RouterLink, useHistory } from 'react-router-dom';
import { AuthenticatedState, useAuthentication } from '../Authentication/AuthWrapper';
import ConversationsIcon from '../Conversations/ConversationsIcon';
import { useAppBarStyles } from './AppBar.styles';
import AppDrawer from './AppDrawer';

const AppMenuRight: React.FC<{ authState: AuthenticatedState }> = ({
    authState: { appProfileExists, oidcProfile, signOut },
}) => {
    const classes = useAppBarStyles();
    const [userMenuOpen, setUserMenuOpen] = useState<boolean>(false);
    const anchorEl = useRef(null);
    const theme = useTheme();
    const { push } = useHistory();

    return (
        <Fragment>
            {appProfileExists && (
                <div className={classes.padding}>
                    <IconButton color="inherit" onClick={() => push('/conversations')}>
                        <ConversationsIcon />
                    </IconButton>
                </div>
            )}
            <div className={classes.padding}>
                <IconButton
                    aria-label="account of current user"
                    aria-controls="menu-appbar"
                    aria-haspopup="true"
                    onClick={() => setUserMenuOpen(!userMenuOpen)}
                    color="inherit"
                    ref={anchorEl}
                >
                    <AccountCircle />
                </IconButton>
            </div>
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
                <Divider style={{ margin: theme.spacing(0.5, 1) }} />
                <MenuItem component={RouterLink} to="/account" onClick={() => setUserMenuOpen(false)}>
                    Account
                </MenuItem>
                <MenuItem onClick={signOut}> Logout</MenuItem>
            </Menu>
        </Fragment>
    );
};

const AppMenu: React.FC = () => {
    const classes = useAppBarStyles();
    const authState = useAuthentication();

    const [drawerOpen, setDrawerOpen] = useState<boolean>(false);

    return (
        <div className={classes.root}>
            <AppDrawer open={drawerOpen} onClose={() => setDrawerOpen(false)} />
            <AppBar
                position="fixed"
                className={classNames(classes.appBar, {
                    [classes.appBarShift]: drawerOpen,
                })}
            >
                <Toolbar>
                    <IconButton
                        edge="start"
                        className={classNames(classes.menuButton, drawerOpen && classes.hide)}
                        color="inherit"
                        aria-label="menu"
                        onClick={() => setDrawerOpen(true)}
                    >
                        <MenuIcon />
                    </IconButton>

                    <Typography variant="h6" className={classes.title}>
                        <Link component={RouterLink} to="/" style={{ color: 'white' }}>
                            BuddyUp!
                        </Link>
                    </Typography>

                    <div className={classes.grow} />

                    {authState.isLoggedIn === true && <AppMenuRight authState={authState} />}
                    {authState.isLoggedIn === false && (
                        <div style={{ marginLeft: 'auto', cursor: 'pointer' }}>
                            <ListItemText onClick={authState.signIn}>Login</ListItemText>
                        </div>
                    )}
                </Toolbar>
            </AppBar>
        </div>
    );
};

export default AppMenu;
