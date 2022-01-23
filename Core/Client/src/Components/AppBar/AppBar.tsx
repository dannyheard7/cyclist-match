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
import { useAuthentication } from '../Authentication/AuthWrapper';
import ConversationsIcon from '../Conversations/ConversationsIcon';
import { useAppBarStyles } from './AppBar.styles';
import AppDrawer from './AppDrawer';

const AppMenu: React.FC = () => {
    const classes = useAppBarStyles();
    const theme = useTheme();
    const { profile, signOut: logout, signInRedirect: login } = useAuthentication();
    const anchorEl = useRef(null);
    const [userMenuOpen, setUserMenuOpen] = useState<boolean>(false);
    const [drawerOpen, setDrawerOpen] = useState<boolean>(false);

    const { push } = useHistory();

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
                    {profile ? (
                        <Fragment>
                            <div className={classes.padding}>
                                <IconButton color="inherit" onClick={() => push('/conversations')}>
                                    <ConversationsIcon />
                                </IconButton>
                            </div>
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
                                        {profile.given_name} {profile.family_name}
                                    </Typography>
                                </MenuItem>
                                <Divider style={{ margin: theme.spacing(0.5, 1) }} />
                                <MenuItem component={RouterLink} to="/account" onClick={() => setUserMenuOpen(false)}>
                                    Account
                                </MenuItem>
                                <MenuItem onClick={() => logout()}> Logout</MenuItem>
                            </Menu>
                        </Fragment>
                    ) : (
                        <div style={{ marginLeft: 'auto', cursor: 'pointer' }}>
                            <ListItemText onClick={() => login()}>Login</ListItemText>
                        </div>
                    )}
                </Toolbar>
            </AppBar>
        </div>
    );
};

export default AppMenu;
