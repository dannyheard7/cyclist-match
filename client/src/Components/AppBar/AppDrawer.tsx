import { Divider, Drawer, IconButton, Link, List, ListItem, ListItemText, useTheme } from "@material-ui/core";
import { ChevronLeft as ChevronLeftIcon, ChevronRight as ChevronRightIcon } from "@material-ui/icons";
import React, { Fragment } from "react";
import { Link as RouterLink } from "react-router-dom";
import { useAuthentication } from "../Authentication/AuthenticationContext";
import { useAppBarStyles } from './AppBar.styles';

const AppDrawer: React.FC<{ open: boolean, onClose: () => void }> = ({ open, onClose }) => {
    const classes = useAppBarStyles();
    const theme = useTheme();
    const { user } = useAuthentication();

    return (
        <Drawer
            className={classes.drawer}
            open={open}
            onClose={() => onClose()}
            variant="persistent"
            classes={{
                paper: classes.drawerPaper,
            }}
        >
            <div className={classes.drawerHeader}>
                <IconButton onClick={() => onClose()}>
                    {theme.direction === 'ltr' ? <ChevronLeftIcon /> : <ChevronRightIcon />}
                </IconButton>
            </div>
            <Divider />
            <List>
                {user && (
                    <Fragment>
                        <Link to="/" component={RouterLink} onClick={() => onClose()}>
                            <ListItem button>
                                <ListItemText>Top Matches</ListItemText>
                            </ListItem>
                        </Link>
                        <Divider />
                    </Fragment>
                )}
                <Link to="/feedback" component={RouterLink} onClick={() => onClose()}>
                    <ListItem button>
                        <ListItemText>Feedback</ListItemText>
                    </ListItem>
                </Link>
                <Link to="/privacy-policy" component={RouterLink} onClick={() => onClose()}>
                    <ListItem button>
                        <ListItemText>Privacy Policy</ListItemText>
                    </ListItem>
                </Link>
            </List>
        </Drawer>
    );
};

export default AppDrawer;
