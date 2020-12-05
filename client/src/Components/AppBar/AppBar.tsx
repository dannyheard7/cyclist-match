import { AppBar, Divider, Drawer, IconButton, Link, List, ListItem, ListItemText, makeStyles, Menu, MenuItem, Toolbar, Typography, useTheme } from "@material-ui/core";
import { AccountCircle, ChevronLeft as ChevronLeftIcon, ChevronRight as ChevronRightIcon, Menu as MenuIcon } from "@material-ui/icons";
import classNames from 'classnames';
import React, { Fragment, useRef, useState } from "react";
import { Link as RouterLink } from "react-router-dom";
import { useAuthentication } from "../Authentication/AuthenticationContext";
import styles from './AppBar.styles';

const useStyles = makeStyles(styles);

const AppMenu: React.FC = () => {
  const classes = useStyles();
  const theme = useTheme();
  const { user, signout: logout, signin: login } = useAuthentication();
  const anchorEl = useRef(null);
  const [userMenuOpen, setUserMenuOpen] = useState<boolean>(false);
  const [drawerOpen, setDrawerOpen] = useState<boolean>(false);

  return (
    <div className={classes.root}>
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
            BuddyUp!
          </Typography>
          <Drawer
            className={classes.drawer}
            open={drawerOpen}
            onClose={() => { setDrawerOpen(false) }}
            variant="persistent"
            classes={{
              paper: classes.drawerPaper,
            }}
          >
            <div className={classes.drawerHeader}>
              <IconButton onClick={() => { setDrawerOpen(false) }}>
                {theme.direction === 'ltr' ? <ChevronLeftIcon /> : <ChevronRightIcon />}
              </IconButton>
            </div>
            <Divider />
            <List>
              {user && (
                <Fragment>
                  <Link to="/" component={RouterLink} onClick={() => { setDrawerOpen(false) }}>
                    <ListItem button>
                      <ListItemText>Top Matches</ListItemText>
                    </ListItem>
                  </Link>
                  <Divider />
                </Fragment>
              )}
              <Link to="/feedback" component={RouterLink} onClick={() => { setDrawerOpen(false) }}>
                <ListItem button>
                  <ListItemText>Feedback</ListItemText>
                </ListItem>
              </Link>
              <Link to="/privacy-policy" component={RouterLink} onClick={() => { setDrawerOpen(false) }}>
                <ListItem button>
                  <ListItemText>Privacy Policy</ListItemText>
                </ListItem>
              </Link>
            </List>
          </Drawer>
          {user ? (
            <Fragment>
              <Typography>{user.profile.given_name} {user.profile.family_name}</Typography>
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
              <Menu
                id="menu-appbar"
                anchorEl={anchorEl.current}
                open={userMenuOpen}
                onClose={() => setUserMenuOpen(false)}
              >
                <MenuItem>
                  <Link to="/account" component={RouterLink}>Account</Link>
                </MenuItem>
                <MenuItem onClick={() => logout()}> Logout</MenuItem>
              </Menu>
            </Fragment>
          ) :
            <div style={{ marginLeft: 'auto', cursor: 'pointer' }}>
              <ListItemText onClick={() => login()}>Login</ListItemText>
            </div>
          }
        </Toolbar>
      </AppBar>
    </div>
  );
};

export default AppMenu;
