import { ChevronLeft as ChevronLeftIcon, ChevronRight as ChevronRightIcon } from '@mui/icons-material';
import { Divider, Drawer, IconButton, Link, List, ListItem, ListItemText, useTheme } from '@mui/material';
import { styled } from '@mui/material/styles';
import React, { Fragment } from 'react';
import { Link as RouterLink } from 'react-router-dom';
import { useAuthentication } from '../Authentication/AuthWrapper';

const PREFIX = 'AppDrawer';
const classes = {
    drawer: `${PREFIX}-drawer`,
    drawerPaper: `${PREFIX}-drawerPaper`,
};

export const DRAWER_WIDTH = 240;
const StyledDrawer = styled(Drawer)({
    [`&.${classes.drawer}`]: {
        width: DRAWER_WIDTH,
    },
    [`& .${classes.drawerPaper}`]: {
        width: DRAWER_WIDTH,
    },
});

const StyledDrawerHeader = styled('div')(({ theme }) => ({
    display: 'flex',
    alignItems: 'center',
    padding: theme.spacing(0, 1),
    ...theme.mixins.toolbar,
}));

const AppDrawer: React.FC<{ open: boolean; onClose: () => void }> = ({ open, onClose }) => {
    const theme = useTheme();
    const { isLoggedIn } = useAuthentication();

    return (
        <StyledDrawer
            className={classes.drawer}
            open={open}
            onClose={() => onClose()}
            variant="persistent"
            classes={{
                paper: classes.drawerPaper,
            }}
        >
            <StyledDrawerHeader>
                <IconButton onClick={() => onClose()} size="large">
                    {theme.direction === 'ltr' ? <ChevronLeftIcon /> : <ChevronRightIcon />}
                </IconButton>
            </StyledDrawerHeader>
            <Divider />
            <List>
                {isLoggedIn && (
                    <Fragment>
                        <Link to="/" component={RouterLink} onClick={() => onClose()} underline="hover">
                            <ListItem button>
                                <ListItemText>Top Matches</ListItemText>
                            </ListItem>
                        </Link>
                        <Divider />
                    </Fragment>
                )}
                <Link to="/feedback" component={RouterLink} onClick={() => onClose()} underline="hover">
                    <ListItem button>
                        <ListItemText>Feedback</ListItemText>
                    </ListItem>
                </Link>
                <Link to="/privacy-policy" component={RouterLink} onClick={() => onClose()} underline="hover">
                    <ListItem button>
                        <ListItemText>Privacy Policy</ListItemText>
                    </ListItem>
                </Link>
            </List>
        </StyledDrawer>
    );
};

export default AppDrawer;
